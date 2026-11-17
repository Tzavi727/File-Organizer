using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileOrganizer.Interfaces;
using FileOrganizer.Records;
using FileOrganizer.Services;

namespace FileOrganizer.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly RuleService _ruleService;
    private readonly IRichPresenceService _richPresenceService;
    private readonly IPreferencesService _preferencesService;

    public SettingsViewModel(RuleService ruleService, IRichPresenceService richPresenceService,
        IPreferencesService preferencesService)
    {
        _ruleService = ruleService;
        _richPresenceService = richPresenceService;
        _preferencesService = preferencesService;
        UpdateRulesList();
        _ruleService.RulesChanged += UpdateRulesList;
    }

    [ObservableProperty] private ObservableCollection<RuleRecord>? _rulesList;

    [NotifyCanExecuteChangedFor(nameof(AddRuleCommand))] [ObservableProperty]
    private string? _keyToAdd;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddRuleCommand))]
    private string? _valueToAdd;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(RemoveRuleCommand))]
    private string? _keyToRemove;

    [ObservableProperty] private bool _richPresenceCheckBox;
    private bool CanAddRule() => !string.IsNullOrWhiteSpace(KeyToAdd) && !string.IsNullOrWhiteSpace(ValueToAdd);
    private bool CanRemoveRule() => !string.IsNullOrWhiteSpace(KeyToRemove);

    [RelayCommand(CanExecute = nameof(CanAddRule))]
    private void AddRule()
    {
        if (string.IsNullOrWhiteSpace(KeyToAdd) || string.IsNullOrWhiteSpace(ValueToAdd))
            return;

        string formattedKey = KeyToAdd.Trim().ToLower();
        string formattedValue = ValueToAdd.Trim().ToLower();

        _ruleService.SetNewRule(formattedKey, formattedValue);
        _ruleService.SaveRules();

        KeyToAdd = null;
        ValueToAdd = null;
    }

    [RelayCommand(CanExecute = nameof(CanRemoveRule))]
    private void RemoveRule()
    {
        if (string.IsNullOrWhiteSpace(KeyToRemove))
            return;

        _ruleService.RemoveRule(KeyToRemove);
        _ruleService.SaveRules();

        KeyToRemove = null;
    }

    private void UpdateRulesList()
    {
        var rawRules = _ruleService.GetAllRules();
        var records = rawRules.Select(r => new RuleRecord(r.Key, r.Value)).ToList();

        if (RulesList == null)
        {
            RulesList = new ObservableCollection<RuleRecord>(records);
        }
        else
        {
            RulesList.Clear();
            foreach (var record in records)
            {
                RulesList.Add(record);
            }
        }
    }

    partial void OnRichPresenceCheckBoxChanged(bool value)
    {
        if (value)
        {
            _richPresenceService.InitializeRpc();
            _richPresenceService.SetIdlePresence();
        }
        else
        {
            _richPresenceService.DisposeClient();
        }

        _preferencesService.IsDiscordEnabled = value;
        _preferencesService.SavePreferences();
    }
}