---
title: ML.NET video tutorials (notes)
tags:
  - ML.NET
  - tutorials
description: Scratchpad for a tutorial series that refuses to be "hello iris" again.
---

This is not a course. It is a pile of notes for videos I *want* to make before I get distracted by another library with `Frank.` in the name.

## Facts: where ML.NET sits in the ecosystem

**ML.NET** is Microsoft's .NET-first ML stack—data loading (`IDataView`), training pipelines, evaluation metrics, and deployment patterns documented under Microsoft Learn. [^mlnet-intro]

**Model Builder** (Visual Studio extension) and the **`mlnet` CLI** automate common scenarios (classification, regression, recommendation, computer vision) so you can iterate without writing boilerplate trainers by hand. [^model-builder]

**ONNX** interoperability matters because academic and industry workflows often train in Python then export; ML.NET documents the **ONNX Runtime** dependency and how to run ONNX models from .NET. [^onnx-mlnet]

**Ethics / optics:** game-adjacent demos (aim bots, wall hacks as *pedagogy*) should foreground responsible disclosure—teach detection and adversarial robustness, not cheat distribution. Microsoft publishes **Responsible AI** principles and Azure Machine Learning documentation on fairness, interpretability, and human review—useful guardrails even for scratch tutorials. [^rai-ms] [^rai-azure]

[^mlnet-intro]: Microsoft Learn — *What is ML.NET?* https://learn.microsoft.com/dotnet/machine-learning/how-does-mldotnet-work

[^model-builder]: Microsoft Learn — *What is Model Builder?* https://learn.microsoft.com/dotnet/machine-learning/automate-training-with-model-builder

[^onnx-mlnet]: Microsoft Learn — *Install ONNX Runtime to use with ML.NET* (ONNX Runtime packages and GPU/CPU notes). https://learn.microsoft.com/dotnet/machine-learning/how-to-guides/install-onnx-runtime

[^rai-ms]: Microsoft — *Responsible AI* overview (principles and practices). https://www.microsoft.com/ai/responsible-ai

[^rai-azure]: Microsoft Learn — *What is responsible machine learning?* (Azure Machine Learning). https://learn.microsoft.com/azure/machine-learning/concept-responsible-machine-learning

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
- [Microsoft Learn — What is ML.NET?](https://learn.microsoft.com/dotnet/machine-learning/how-does-mldotnet-work)
- [Microsoft Learn — Model Builder](https://learn.microsoft.com/dotnet/machine-learning/automate-training-with-model-builder)
- [Microsoft Learn — ML.NET CLI](https://learn.microsoft.com/dotnet/machine-learning/automate-training-with-cli)
- [Microsoft Learn — Install ONNX Runtime to use with ML.NET](https://learn.microsoft.com/dotnet/machine-learning/how-to-guides/install-onnx-runtime)
- [Microsoft Learn — Save and load a model in ML.NET](https://learn.microsoft.com/dotnet/machine-learning/how-to-guides/save-load-machine-learning-models-ml-net) (general persistence; ONNX loading is covered in ONNX-specific topics above)
- [ONNX Runtime (GitHub)](https://github.com/microsoft/onnxruntime)
- [Microsoft — Responsible AI](https://www.microsoft.com/ai/responsible-ai)
- [Microsoft Learn — Responsible machine learning (Azure ML)](https://learn.microsoft.com/azure/machine-learning/concept-responsible-machine-learning)
