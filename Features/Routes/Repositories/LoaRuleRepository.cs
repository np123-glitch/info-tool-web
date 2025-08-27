﻿using ZmaReference.Features.Routes.Models;

namespace ZmaReference.Features.Routes.Repositories;

public class LoaRuleRepository
{
    private List<LoaRule> _repository = new List<LoaRule>();
    
    public void AddRule(LoaRule rule) => _repository.Add(rule);
    
    public void AddRules(IEnumerable<LoaRule> rules) => _repository.AddRange(rules);

    public IEnumerable<LoaRule> GetAllRules() => _repository;

    public void ClearRules() => _repository.Clear();
}