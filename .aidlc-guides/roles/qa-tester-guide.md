# QA/Tester Guide: Working with AI-Evolved SDLC

Your agent automatically follows testing requirements from .ai/rules/. Your workflow depends on your team's Volaris maturity level.

## Your Role by Maturity Level

| Stage | Volaris L1 (AI Assisted) | Volaris L2 (AI Directed) | Volaris L3 (AI Delegated) |
|-------|--------------------------|--------------------------|---------------------------|
| Test Strategy | Define strategy with AI help | Review AI-generated strategy | Approve agent strategy |
| Test Writing | Write tests with AI help | Review AI-generated tests | Review agent test results |
| PBT | Write PBT with AI help | Review AI-generated PBT | Review PBT results |
| Coverage | Check coverage with AI help | AI maps coverage to requirements | Agent maintains coverage |
| Exploratory | Manual with AI suggestions | Manual with AI suggestions | Manual with AI suggestions |

---

## Volaris L1: AI Assisted

You write tests. AI helps with suggestions, edge cases, and scaffolding.

### Daily Prompts
```
Help me write tests for [component]
```
```
Suggest edge cases for [feature]
```
```
Generate test scaffolding for [integration point]
```
```
Check test coverage for [component]
```

---

## Volaris L2: AI Directed

You set intent. AI generates comprehensive tests. You review and validate.

### Daily Prompts
```
Generate tests for [feature/component]
```
```
Run property-based tests for [component] with 100+ iterations
```
```
Validate test coverage for [feature] against requirements
```
```
Suggest exploratory test scenarios for [feature]
```
```
Pre-release validation: run all tests, report gaps
```

---

## Volaris L3: AI Delegated

Agent generates and runs tests autonomously. You review results and do exploratory testing.

### Daily Prompts
```
Review the agent's test results for [feature]
```
```
The agent's tests are missing: [scenarios]
```
```
Show me coverage report for [feature]
```

---

## Tips

- At all levels: exploratory testing is where your human creativity adds most value
- At L1: AI helps you write better tests faster
- At L2: AI writes tests from specs, you validate they test the right things
- At L3: AI writes and runs tests, you validate results and explore edge cases
- Property-based tests are powerful at L2+ - AI generates them from design correctness properties
