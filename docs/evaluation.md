# AI Evaluation

The goal of the AI feature is to generate useful text based on a user prompt. The output should be helpful, relevant and safe enough to show to a user, but it should still be reviewed by a human before being used as final content.

## Quality criteria

I evaluated the AI output using these criteria:

- Relevance: the answer should match the prompt.
- Correctness: the answer should not invent specific facts.
- Language: the answer should use the same language as the prompt.
- Consistency: the answer should follow the expected structure.
- Clarity: the answer should be easy to understand.
- Hallucinations: the answer should avoid made-up details.
- Uncertainty: if the AI is unsure, it should say so instead of guessing.

## Test prompt 1

Prompt:

```text
Write a short product description for a black coffee mug.
```

Result:

The AI generated a short and clear product description. The text was relevant and useful for a simple product page.

What worked well:

- The answer matched the prompt.
- The language was clear.
- The output was short and easy to use.
- It did not include unnecessary technical details.

What could be improved:

- The text was a bit generic.
- It could have been more specific if the prompt included target audience, material or style.

Conclusion:

The AI works well for simple marketing text, but the input prompt needs enough detail to make the result more unique.

## Test prompt 2

Prompt:

```text
Write a company description for a fictional company called Line 5 Entertainment. Do not invent exact numbers, awards or real customers.
```

Result:

The AI generated a general company description and avoided exact claims such as revenue, awards or real customers.

What worked well:

- The output stayed relevant to the fictional company.
- The AI did not include fake customer names or specific awards.
- The text was usable as a draft.

What could be improved:

- The result was still quite broad.
- A user could easily get a more specific result by adding industry, tone and target audience.

Conclusion:

The AI can handle fictional content, but it needs clear instructions to avoid hallucinated facts.

## Test prompt 3

Prompt:

```text
Give advice about whether a small business should use AI-generated legal text directly in contracts.
```

Result:

The AI explained that AI-generated legal text should not be used directly without review. It suggested that a qualified person should check the text before it is used.

What worked well:

- The answer was cautious.
- It did not pretend to replace legal advice.
- It explained the risk in a clear way.

What could be improved:

- The answer could be more specific about what parts of a contract are risky.
- It should continue to avoid giving final legal advice.

Conclusion:

The AI is useful for drafts and explanations, but it should not be trusted as the final source for legal, medical, financial or other high-risk decisions.

## Limitations

The AI service has several limitations:

### Hallucinations

The AI can generate information that sounds correct even when it is not. This is especially risky when the prompt asks for facts, numbers, legal advice, technical details or real-world references.

### Bias

The output may reflect bias from the data the model was trained on. This can affect tone, assumptions and recommendations.

### Prompt sensitivity

Small changes in the prompt can change the answer a lot. A vague prompt often gives a vague result.

### Garbage in, garbage out

If the input prompt is unclear, incorrect or missing important context, the generated output will usually be worse.

### No guaranteed fact checking

The AI does not guarantee that all statements are correct. Important facts should be checked against reliable sources.

## What I changed after testing

After testing the prompts, I made the AI instructions more strict.

The developer instructions should make the AI:

- answer in the same language as the user prompt
- avoid inventing facts
- say when it is unsure
- keep the output clear and not too long
- follow a simple expected structure when possible

I also improved error handling so that failed AI requests return safe `ProblemDetails` responses instead of raw internal errors.

## Final conclusion

The AI service is useful for generating drafts, summaries, product descriptions and other low-risk text.

It is not suitable as the only source for important decisions, legal text, medical advice, financial advice or factual claims that require verification.

The generated content should be treated as a draft and reviewed by a human before it is published or used.
