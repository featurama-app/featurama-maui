using Featurama.Maui.Models;
using Featurama.Maui.UI.Strings;
using Featurama.Maui.UI.Theme;
using Featurama.Maui.UI.Utils;
using Featurama.Maui.UI.Views;

namespace Featurama.Maui.UI;

public sealed class FeaturamaPage : ContentPage
{
    private readonly FeaturamaTheme _theme;
    private readonly FeaturamaStrings _strings;
    private readonly Action? _onClose;
    private readonly string _voterId;

    private string _activeFilter = "new";
    private bool _isAdding;
    private bool _isLoading;
    private string? _error;
    private PaginatedResponse<FeatureRequest>? _data;
    private readonly HashSet<string> _votingIds = new();

    private readonly VerticalStackLayout _listContainer;
    private readonly VerticalStackLayout _mainLayout;
    private CreateRequestView? _createForm;
    private BrandingView? _brandingView;

    public FeaturamaPage(FeaturamaPageOptions? options = null)
    {
        var opts = options ?? new FeaturamaPageOptions();
        var accentColor = opts.AccentColor ?? Color.FromArgb("#007AFF");
        _theme = ThemeFactory.Create(accentColor, opts.ColorScheme);
        _strings = opts.Strings ?? new FeaturamaStrings();
        _onClose = opts.OnClose;
        _voterId = VoterIdProvider.GetOrCreate();

        NavigationPage.SetHasNavigationBar(this, false);
        BackgroundColor = _theme.Background;

        var header = new HeaderView(_theme, _strings, _onClose, OnAdd);
        var filterTabs = new FilterTabsView(_theme, _strings);
        filterTabs.FilterChanged += OnFilterChanged;

        _listContainer = new VerticalStackLayout();
        _mainLayout = new VerticalStackLayout
        {
            Children = { header, filterTabs },
        };

        var scrollView = new ScrollView
        {
            Content = _listContainer,
            VerticalOptions = LayoutOptions.Fill,
        };

        _brandingView = new BrandingView(_theme)
        {
            Margin = new Thickness(0, 0, 0, 16),
            VerticalOptions = LayoutOptions.End,
            HorizontalOptions = LayoutOptions.Center,
            InputTransparent = false,
        };

        var contentGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
            },
            Children = { _mainLayout, scrollView },
        };
        Grid.SetRow(_mainLayout, 0);
        Grid.SetRow(scrollView, 1);

        // Overlay grid to float branding badge at bottom center
        Content = new Grid
        {
            Children = { contentGrid, _brandingView },
        };

        Loaded += async (_, _) =>
        {
            await LoadConfig();
            await LoadData();
        };
    }

    private void OnAdd()
    {
        if (_isAdding) return;
        _isAdding = true;
        _createForm = new CreateRequestView(_theme, _strings);
        _createForm.SubmitRequested += OnSubmitRequest;
        _createForm.CancelRequested += OnCancelCreate;
        if (_mainLayout.Children.Count > 2)
            _mainLayout.Children.Insert(2, _createForm);
        else
            _mainLayout.Children.Add(_createForm);
    }

    private void OnCancelCreate(object? sender, EventArgs e)
    {
        RemoveCreateForm();
    }

    private void RemoveCreateForm()
    {
        if (_createForm != null)
        {
            _mainLayout.Children.Remove(_createForm);
            _createForm = null;
        }
        _isAdding = false;
    }

    private async Task OnSubmitRequest(string title, string description)
    {
        await Featurama.CreateFeatureRequestAsync(title, description, _voterId);
        RemoveCreateForm();
        await LoadData();
    }

    private async void OnFilterChanged(object? sender, string filter)
    {
        _activeFilter = filter;
        await LoadData();
    }

    private async Task LoadConfig()
    {
        try
        {
            var config = await Featurama.GetProjectConfigAsync();
            if (_brandingView != null)
                _brandingView.IsVisible = config.Branding.ShowBranding;
        }
        catch
        {
            // Config fetch failed — keep branding visible (default)
        }
    }

    private async Task LoadData()
    {
        _isLoading = true;
        _error = null;
        RebuildList();

        try
        {
            _data = await Featurama.GetFeatureRequestsAsync(pageSize: 50, filter: _activeFilter);
        }
        catch (Exception ex)
        {
            _error = ex.Message;
        }
        finally
        {
            _isLoading = false;
        }

        RebuildList();
    }

    private void RebuildList()
    {
        _listContainer.Children.Clear();

        if (_isLoading && _data == null)
        {
            _listContainer.Children.Add(new ActivityIndicator
            {
                IsRunning = true,
                Color = _theme.Accent,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 60),
            });
            return;
        }

        if (_error != null && _data == null)
        {
            _listContainer.Children.Add(new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 60),
                Spacing = 16,
                Children =
                {
                    new Label
                    {
                        Text = _strings.Error,
                        TextColor = _theme.TextSecondary,
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new Button
                    {
                        Text = _strings.Retry,
                        TextColor = _theme.AccentForeground,
                        BackgroundColor = _theme.Accent,
                        CornerRadius = 8,
                        Command = new Command(async () => await LoadData()),
                    },
                },
            });
            return;
        }

        if (_data != null && _data.Items.Count == 0 && !_isLoading)
        {
            _listContainer.Children.Add(new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 60),
                Spacing = 8,
                Children =
                {
                    new Label
                    {
                        Text = _strings.Empty,
                        TextColor = _theme.TextSecondary,
                        FontSize = 16,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                    new Label
                    {
                        Text = _strings.EmptyHint,
                        TextColor = _theme.TextSecondary,
                        FontSize = 14,
                        HorizontalTextAlignment = TextAlignment.Center,
                    },
                },
            });
            return;
        }

        if (_data != null)
        {
            var layout = new VerticalStackLayout
            {
                Padding = new Thickness(16, 12, 16, 40),
            };
            foreach (var request in _data.Items)
            {
                var id = request.Id.ToString();
                var card = new RequestCardView(
                    _theme,
                    _strings,
                    request,
                    _votingIds.Contains(id),
                    () => _ = HandleToggleVote(id)
                );
                layout.Children.Add(card);
            }
            _listContainer.Children.Add(layout);
        }
    }

    private async Task HandleToggleVote(string requestId)
    {
        if (_votingIds.Contains(requestId)) return;
        _votingIds.Add(requestId);
        RebuildList();

        try
        {
            await Featurama.ToggleVoteAsync(Guid.Parse(requestId), _voterId);
            await LoadData();
        }
        finally
        {
            _votingIds.Remove(requestId);
        }
    }
}
