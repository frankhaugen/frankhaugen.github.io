<Query Kind="Statements">
  <NuGetReference>RhuBulletSharp</NuGetReference>
  <Namespace>BulletSharp</Namespace>
  <Namespace>BulletSharp.SoftBody</Namespace>
  <Namespace>System.Globalization</Namespace>
  <CopyLocal>true</CopyLocal>
</Query>



var box = new Box2DShape(1);

var motionState = new DefaultMotionState();

var constructionInfo = new RigidBodyConstructionInfo(10, motionState, box);



var body = new RigidBody(constructionInfo);





for (int i = 0; i < 100; i++)
{
    
    body.Dump();
    
    
    Thread.Sleep(100);
}
