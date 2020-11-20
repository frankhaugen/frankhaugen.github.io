# Episode 0
This is a simple introduction to ML.net, what it is and can do, and more importantly, what it's not and cannot do. No programming or installation instructions in this one.

## What it is
1. ML.net is the open-sourcing of Microsoft's internal Machine Learning tools
2. It contains basic functionality for data-driven machine learning
3. There is a growing community of developers contributing to the repo, making improvements every day

## What it's not
1. At this point, ML.net isn't the "all-in-one"-solution to your ML needs.
    - A sidenote here is that all these ML frameworks are the "child-friendly" implementations of Machine Learning, and if a company is doing something very specific that needs to be optimized, they should consult with someone who has a PHD in statistics to make sure they are doing it right. ML.net will probably be a good fit for data-driven businesses, but finding the correct algorithms is important
2. A neural-network framework like Tensorflow can be used for training and the resultant model consumed by ML.net, but at this time, there are no NEAT or Q-Learning in ML.net, (probably will be in not too long)
    - If you are a .net developer, fear not, because there are few concepts that someone haven't implemented in some library. NeatSharp is a library that implements NEAT in C#, and is distributed as a Nuget . There are libraries like Accord.net that has a Q Learning library, but the Accord.net is being archived, and what will happen now to it is unclear, (it will not be removed, but it's adrift without any clear goal or purpose, so we will see)
3. ML.net isn't very intuitive from a "code first"-perspective. If you are new to the framework, you should use the Model Builder to get a basic project going, and then modify it to your needs from there. The reason is that ML.net has a lot of setup when implement it, and if you don't know the "things", you cannot guess everything you need to