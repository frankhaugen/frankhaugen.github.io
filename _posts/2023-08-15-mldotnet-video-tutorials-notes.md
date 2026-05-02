---
title: ML.NET video tutorials (notes)
tags:
  - ML.NET
  - tutorials
description: Scratchpad for a tutorial series that refuses to be "hello iris" again.
---

This is not a course. It is a pile of notes for videos I *want* to make before I get distracted by another library with `Frank.` in the name.

## Facts: where ML.NET sits in the ecosystem

**ML.NET** is Microsoft's .NET-first ML stack—data loading (`IDataView`), training pipelines, evaluation metrics, and deployment patterns documented under Microsoft Learn.[^mlnet-intro]

**Model Builder** (Visual Studio extension) and the **`mlnet` CLI** automate common scenarios (classification, regression, recommendation, computer vision) so you can iterate without writing boilerplate trainers by hand.[^model-builder]

**ONNX** interoperability matters because academic + industry workflows often train in Python then export; ML.NET explicitly documents loading ONNX models for inference scenarios inside .NET services.[^onnx-mlnet]

**Ethics / optics:** game-adjacent demos (aim bots, wall hacks as pedagogy) should foreground responsible disclosure—teach detection / adversarial robustness, not cheat distribution. Microsoft's Responsible AI tooling and guidance live adjacent to Azure ML docs; even scratch tutorials benefit from stating intent.

[^mlnet-intro]: Microsoft Learn — *What is ML.NET?*. https://learn.microsoft.com/dotnet/machine-learning/how-does-mldotnet-work

[^model-builder]: Microsoft Learn — *What is Model Builder?*. https://learn.microsoft.com/dotnet/machine-learning/automate-training-with-model-builder

[^onnx-mlnet]: Microsoft Learn — *Load ONNX models in ML.NET*. https://learn.microsoft.com/dotnet/machine-learning/how-to-guides/save-load-machine-learning-models-ml-net

## Concept

Most ML.NET content falls into two buckets:

1. "Data scientists, look — C# exists."
2. "C# devs, you can be ML people too, here is a spreadsheet and a dream."

Both are fine. Neither is what I want to build. I want messier, more playful stuff: things you can break, things that feel like games, things where "don't try this in prod" is the whole point. Yes, that includes the morally questionable territory of aim-bot-shaped pedagogy — *teaching*, not cheating. Calm down.

## Episode 0

Draft / scratch lives in the monorepo of chaos:

- [Episode 0 (markdown) on GitHub](https://github.com/frankhaugen/frankhaugen/blob/main/unsorted/from-github-pages/mldotnet-tutorials/mldotnet-tutorial-ep0.md)

If that link rots, the internet deserved it.

## References

- [Microsoft Learn — ML.NET documentation hub](https://learn.microsoft.com/dotnet/machine-learning/)
- [Microsoft Learn — Model Builder](https://learn.microsoft.com/dotnet/machine-learning/automate-training-with-model-builder)
- [Microsoft Learn — ML.NET CLI](https://learn.microsoft.com/dotnet/machine-learning/automate-training-with-cli)
- [Microsoft Learn — ONNX + ML.NET](https://learn.microsoft.com/dotnet/machine-learning/how-to-guides/save-load-machine-learning-models-ml-net)
- [ONNX runtime (GitHub)](https://github.com/microsoft/onnxruntime)
