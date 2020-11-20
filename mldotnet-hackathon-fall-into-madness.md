# Falling into madness during the 2020 ML.net hackathon
"Do something with ml.net" was the task given by the cool guys and gals from the ml.net community. That's easy enough, right? WRONG!!! It's stressful and frustrating.

The idea was to use a game that have been re-created as Open Source in C#. Two alternatives was the most promising: OpenRA, and ManagedDoom, (Red Alert and Doom). We quickly found it to be above our skill level to to something with Red Alert, so Doom it was, and what an apt title for our little team.

## ML.net plays Doom
Having Ml.net play Doom seemed to be fun, and educational, it would also be a rare addition to the content people have been making, (not a lot of ml.net have been used for gaming, so it could really be something new and exciting). We found a great tutorial with code for object detection using onnx-models in ml.net, and having an algorithm that remembers how it moves forward, detects an enemy and learns that pattern can be achieved by other

## The team
We were 4 people, (and beer), three "full" developers and one junior. The junior had some tutorials with machine learning under her belt and C# skills. One of the devs was mainly a Kotlin dev, and the remaining two was .net developers, one working for an AI/ML based fintech company. We called ourselves "Omega Force" because Frank already had created a GitHub organization with that name, and it's cool. A small snag became apparent pretty quickly and that was that the Kotlin dev expected C# to be pretty simple, but found it confusing, and also other things came up that he had to take care of. One of the C# devs needed some RnR and threw in the towel almost immediately. There we were with 30% of the C# skills gone, and the moral-boosting Kotlin dev was suddenly also unavailable...

## Keep calm and code on
A bit frustrated, we started attacking the issue, and five minutes after a good nights sleep it didn't seem too bad, we had a nice example of how to use ml.net to identify objects in an image. The example used an object labeling tool called `labelimg` that require one to go through every image and created boxes around objects that are then tagged, and it saves an xml-file per image. Having sprites from Doom made it really easy to just write a few lines of C# code and make the XML based on the entire image being the "box" to be labeled. 1400 sprites took 15 minutes to process:2 minutes of manual tagging, 1 minute of thinking 10 minutes of programming, and 2 minutes for the app to run. Armed with XML-files and the sprites we got going trying to setup Anaconda, Tensorflow, NVidia CUDA-stuff, and a bunch more...

it didn't go well...

## Troubles with Tensorflow
When you look at tutorials and documentation it looks so easy, but we tried many times, but nothing seems to work... What do you do then??? Drink a lot, cry a lot, and yell at people for no reason a lot. It's one thing to have a hard time doing something difficult, but when it seems so easy in the tutorials, making it seem like it takes ten minutes to get started. But error message like Python not finding `object_detection.utils` leads down a path of misery and self-loathing as you read the snarky answers to anyone brave enough to have asked a question online about it, (when you have done a `pip install object_detection` without errors, you expect it to just work). You finally build up the curage to ask the question to ask a question on stack overflow, and then there are no shortage of downvotes, (a small line encuraging downvoters to comment to help improve the question was edited away almost instantly), but you get that one comment that gets you over the hurdle, (turned out that ProtoBuf didn't install correctly, and the errors are easy to miss).

A few more hours go on and we finally can start training... FOUR DAYS INTO THE HACKATHON!!! And the defaults take about 18 hours to train on a 95th percentile powerful computer... And paying a couple of hundred dollars for cloud computing to fastrack it, isn't feasable

## Houston we have a model
So finally we had a model, (we created a barely trained one just to experiment), now it's just converting it from TF to ONNX, and that can't be hard, right? It's not according to the documentation... And we run it, it takes 15 minutes, with the Red Bull to blood ratio being about 2:1 in our bodies, we gather around and and then our moment of elation ends when the model.onnx file isn't there... Maybe it was saved someplace weird? Start searching C:\ for `*.onnx`, and get a bunch of false positives... Now the tears are starting to well up as one realizes that there are not a single line of actual, functional, (as in working, not paradigmic), code and there is 30 hours left of the hackathon... When looking at the source code for `tf2onnx`, it's aparent that it just quits at some random point without an error message or any kind of logging... Should we give up? Should we surrender to the dispair?? It's so tempting...

## Scope narrowing
Having reduced to the bearest of scopes, it seems like a win to have created an aimbot, and learned to at least use the tools available, but even that seems too abitious 30 hours before the deadline... Maybe this rant, blog, whatever, is the only actual result of the entire catastrophy

## Exploring the Tensorflow model
Using Netron to view the created `.pb`-model file, which took 17 hours to train, 3 days to even start the training, and now it seems wrong, having 1400 "nodes" as inputs, error messages about unknown types and takes ten minutes to open. Something is crazy wrong and noone have any ideas... The internet keeps presenting these steps as easy and straight forward yet it's anything but. We had the idea that since Tensorflow and all this ML stuff is presented as such plug-n-play -processes that anyone can do it, it shouldn't be too hard to get a model going. But with two people spending hours each day reading, experimenting, and watching online tutorials, the easy part seems to be a marketing ploy from Google, and that Google as a service is hiding anything indicating it's not super-duper-awesome-easy to do ML training and conversion... Also, I HATE PYTHON! The language is clumsy, (significant whitespace is an abomination, and herecy against the natural order of the univers), but the entire ecosystem is so fragile that you update one thing, that updates something else, that then breaks the functionality of the `.py` file you try to run, and you end up going into actual libraries and tweaking code... How can people rave about Python as the greatest thing since the invention of fire, I have no idea...

## What to do now?
The original MASH movie, (that spawned the excellent TV show with the same name), has a theme song that has been stuck in my head for most of this process, it's lyrics is very fitting for the situation, (look it up). Maybe, just maybe, there is a chance that someone can help us? Mayby @luisquintanilla or @briacht can assist? They seem like very nice an knowledable people, only one snag.. How to contack them? O.o

Turns out email is a powerful tool! Bri was reasonably quick to answer and forward our plea for help to Luis, who was very helpful, (I have yet to interact with a Microsoft employee that isn't service-minded and likable). Luis got us "on track" but with very little time remaining, and only 1.5 people of the initial 4 left on the team, delivering anything seems impossible.

## Oh! Microsoft's Model Builder has object detection <3
So as a last ditch effor to salvage this situation, I throw toghether some code to make Vott Json for my sprites, and lob it up into Azure, hoping I can afford it, (turns out that it's not possible to setup anything really expensive for some reason). three hours later, it fails... I reduce the data complexity by only having one "label" and about 600 sprites. It takes almost two hours to finish, but when I plug in the model into my simplified WPF-app, (uses a still-image during development, in stead of capturing the game using OpenCvSharp), it give a 0,999876 score on the top left quadrant of the image. The area the model claim to have an enemy is  


## The surrender
Now it's just me at 04:00 UTC with 4 hours left until the deadline and all there is to show for all this time is a lot of frustration, regret and self-loathing. I didn't even get to drink as much beer as I planned... There will be no entry


, at least I have a lot of experiences to bring with me. 














## Sources
|Description|Link|
|---|---|
||[]()|
|tf2onnx|[https://github.com/onnx/tensorflow-onnx](https://github.com/onnx/tensorflow-onnx)|
|Tensorflow object detection tutorial|[https://tensorflow-object-detection-api-tutorial.readthedocs.io/en/latest/training.html](https://tensorflow-object-detection-api-tutorial.readthedocs.io/en/latest/training.html)|
|Wolfenstein 3D neat article|[https://vbstudio.hu/en/blog/year-2019/20190317-Growing-an-AI-with-NEAT](https://vbstudio.hu/en/blog/year-2019/20190317-Growing-an-AI-with-NEAT)|
|Screen recorder library|[https://github.com/sskodje/ScreenRecorderLib](https://github.com/sskodje/ScreenRecorderLib)|
|ML.net object detection tutorial|[https://github.com/fralik/ObjectDetection-MLNet](https://github.com/fralik/ObjectDetection-MLNet)

## Useful snippets
|Description||
|---|---|
|Convert .pd to .onnx comand|`python -m tf2onnx.convert  --input /exported-models/doom1/saved_model/saved_model.pb --inputs input_1:0 --outputs probs/Softmax:0 --output model2.onnx`|
|Train model|`python model_main_tf2.py --model_dir=models --pipeline_config_path=models/pipeline.config`|
|Convert `*-xml`s to tfrecords|`python generate_tfrecord.py -x images/train -l annotations/label_map.pbtxt -o annotations/train.record`|