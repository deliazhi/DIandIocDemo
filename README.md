
还记得之前介绍工厂模式之抽象工厂吗？
当时，我们是获取两家OTA的国内游线路列表，代码如下：
```
Console.WriteLine("=========抽象工厂模式==========");
ITravelFactory tc_factory = new TC_Factory();
ITravelFactory xc_factory = new XC_Factory();
IDomesticTravel tc_domesticTravel = tc_factory.createDomesticTravel();
IDomesticTravel xc_domesticTravel = xc_factory.createDomesticTravel();
string tc_domesticLineList = tc_domesticTravel.GetLineList();
string xc_domesticLineList = xc_domesticTravel.GetLineList();
Console.WriteLine(tc_domesticLineList);
Console.WriteLine(xc_domesticLineList);
```
实际上，客人一般一次只会获取一家OTA的线路列表。有同学说，很简单呀，我只提供一家OTA的线路列表就好啦。可是，如果客人下次又想获取另外一家OTA的线路列表了呢？事实上，我们没办法知道客人什么时候想要哪家OTA的线路列表，但我们可以把所有的OTA商家写到一个配置文件里，然后通过一个工厂容器类去读取配置文件，得到确定的OTA商家，然后返回确定的OTA商家工厂，最后获取相应商家的线路列表即可。用UML图表示如下：
<!--more-->
![UML示意图](/img/UML示意图.png)

从这个UML示意图可以看出，基本的代码同之前抽象工厂的代码一模一样，不同的地方在于多了一个FactoryContainer，以及main函数中通过这个FactoryContainer去创建配置文件中配置的工厂。所以这里，我们就只写一下FacoryContainer以及main函数的代码：
FactoryContainer的代码：
```
internal class FactoryContainer
{
    public static IFactory factory { get; private set; }
    static FactoryContainer()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("factoryConfig.json");
        var configuration = builder.Build();
        string configValue = configuration["factory"];

        if(configValue=="TC"){
            factory = new TC_Factory();
        }
        else if(configValue=="XC"){
            factory = new XC_Factory();
        }
        else{
            throw new Exception("Factory Init Error");
        }
    }
}
```
main函数的代码：
```
Console.WriteLine("=========依赖注入的方式==========");
IFactory factory = FactoryContainer.factory;
IDomesticTravel domesticTravel = factory.createDomesticTravel();
Console.WriteLine(domesticTravel.GetLineList()); 
IOutboundTravel outboundTravel = factory.createOutboundTravel();
Console.WriteLine(outboundTravel.GetLineList()); 
ICruiseTravel cruiseTravel = factory.createCruiseTravel();
Console.WriteLine(cruiseTravel.GetLineList()); 
```
运行结果：
```
=========依赖注入的方式==========
您获取到TC的国内游线路列表啦！
您获取到TC的出境游线路列表啦！
您获取到TC的邮轮游线路列表啦！
```
这里我们使用了JSON格式的配置文件factoryConfig.json，文件内容如下：
```
{
  "factory": "TC"
}
```
并且使用了ASP.NET Core读取JSON配置文件的方式去获取配置值：
```
var builder = new ConfigurationBuilder()
            .AddJsonFile("factoryConfig.json");
var configuration = builder.Build();
string configValue = configuration["factory"];
```
ASP.NET Core除了可以读取JSON格式的配置文件，还可以读取XML格式的配置文件等等，具体的可以查看ConfigurationBuilder下的各种Add方法。
现在，我们可以不修改程序，只修改一下配置文件，将配置文件的值改为XC，运行后结果如下：
```
=========依赖注入的方式==========
您获取到XC的国内游线路列表啦！
您获取到XC的出境游线路列表啦！
您获取到XC的邮轮游线路列表啦！
```
这便是多态的体现和依赖注入的效果。
啊？什么！我们已经使用了依赖注入了吗？
### 依赖获取
对，没错！我们已经使用了依赖注入，不过这里的依赖注入准确点讲应该是依赖获取，也就是系统中提供一个获取点（FactoryContainer），客户类(Main函数)依赖服务类的接口（IFactory）。当客户类需要服务类时，从获取点主动获取指定的服务类，具体的服务类类型由获取点的配置决定。
依赖注入不止依赖获取这一种方式，还有Setter注入（属性注入）和构造注入。
### 属性注入（Setter注入）
定义一个抽象的旅游产品接口ITravelProd：
```
public interface ITravelProd
{
    string GetLineList();
}
```
定义一个抽象的旅游产品类：
```
public class TravelProd
{
    ITravelProd _itravelproduct;
    public ITravelProd itravelproduct
    {
        set => this._itravelproduct = value;
        get => this._itravelproduct;
    }
    public void GetLineList()
    {
        Console.WriteLine(_itravelproduct.GetLineList());
    }
}
```
TravelProd类依赖ITravelProd接口，怎么依赖的呢？我们给TravelProd类定义一个类型为ITravelProd的属性，然后我们定义具体的旅游产品类，且具体类要继承ITravelProd接口：
```
public class DomesticTravel : ITravelProd
{
    public string GetLineList() => "国内游的线路列表";
}
    public class OutboundTravel:ITravelProd
{
    public string GetLineList() => "出境游的线路列表";
}
    public class CruiseTravel:ITravelProd
{
    public string GetLineList() => "邮轮游的线路列表";
}
```
然后在Main函数中，我们首先定义具体的旅游产品实例，然后定义一个抽象旅游产品实例，通过给这个抽象实例的属性赋具体旅游产品的实例，从而达到获取不同旅游产品线路列表的功能。具体代码如下：
```
Console.WriteLine("=========属性注入==========");
ITravelProd domestic = new DomesticTravel();
ITravelProd outbound = new OutboundTravel();
ITravelProd cruise = new CruiseTravel();

TravelProd travel = new TravelProd();
travel.itravelproduct = domestic;
travel.GetLineList();
travel.itravelproduct = outbound;
travel.GetLineList();
travel.itravelproduct = cruise;
travel.GetLineList();
```
运行结果：
```
=========属性注入==========
国内游的线路列表
出境游的线路列表
邮轮游的线路列表
```
属性依赖就是通过属性来传递依赖。
### 构造注入
构造注入和属性注入很相似，不同点在于这次我们使用构造函数进行注入，所以我们修改一下抽象的旅游产品类：
```
public class TravelProduct
{
    ITravelProduct _itravelprod;
    public TravelProduct(ITravelProduct itravelprod){
        _itravelprod = itravelprod;
    }
    public void GetLineList()
    {
        Console.WriteLine(_itravelprod.GetLineList());
    }
}
```
然后Main函数中，实例化抽象的旅游产品类时就传入具体的旅游产品实例即可：
```
Console.WriteLine("=========构造注入==========");
ITravelProduct construction_domestic = new ConstructionInjection.DomesticTravel();
ITravelProduct construction_outbound = new ConstructionInjection.OutboundTravel();
ITravelProduct construction_cruise = new ConstructionInjection.CruiseTravel();

TravelProduct travel_domestic = new TravelProduct(construction_domestic);
travel_domestic.GetLineList();
TravelProduct travel_outbound = new TravelProduct(construction_outbound);
travel_outbound.GetLineList();
TravelProduct travel_cruise = new TravelProduct(construction_cruise);
travel_cruise.GetLineList();
```
运行结果：
```
=========构造注入==========
国内游的线路列表
出境游的线路列表
邮轮游的线路列表
```

细心的人也许已经发现上面介绍依赖获取的时候，FactoryContainer里有不符合OCP的if...else...，其实这个问题只要使用反射就可以解决了。具体怎么实现，我们在工厂模式中其实也介绍过，这里就不再重复介绍了。

至此，关于依赖注入基本就介绍完了。
那么，什么又是Ioc呢？
依赖获取是我们手动创建依赖对象，然后根据配置值获取指定的服务类；属性注入和构造注入都是我们手动创建依赖对象，然后把依赖对象传给属性，或者传给构造函数。然而，对于大型项目来说，相互依赖的组件会比较多。如果还用手动的方式，自己来创建和注入依赖的话，显然效率很低，而且往往还会出现不可控的场面。因此，IoC容器诞生了，所以，本质上上讲IoC是实现DI的框架，它包含以下几个功能：
动态创建、注入依赖对象。
管理对象生命周期。
映射依赖关系。
目前，比较流行的Ioc容器有以下几种：
+ Autofac:  http://autofac.readthedocs.io/en/latest/
+ Unity：  http://unity.codeplex.com/
+ Spring.NET： http://www.springframework.net/
下面我们以Autofac为例，来改造一下我们的手动注入：
step1:添加Autofac引用
step2:创建一个ContainerBuilder
```
var builder = new ContainerBuilder();
```
step3:注册组件
```
builder.RegisterType<SetterInjection.DomesticTravel>();
builder.RegisterType<SetterInjection.OutboundTravel>();
builder.RegisterType<SetterInjection.CruiseTravel>();
```
我们这里直接用类名去注册组件，并且把用到的三个类都注册了。
Autofac的注册方式有很多种，目前只搞懂了用接口去注册和用类名去注册，其他属性注册，构造注册，lambda注册等等我还没搞明白，还需进一步研究。
step4:创建容器，其实就是build一下上面创建的ContainerBuilder对象
```
var container = builder.Build();
```
需要注意的是，这个ContainerBuilder对象只可以被build一次，所以在build之前要把所有你要用到的组件都注册进去哦~
step5:从容器里面获取实例
```
var domesticContainer=container.Resolve<SetterInjection.DomesticTravel>();
var outboundContainer = container.Resolve<SetterInjection.OutboundTravel>();
var cruiseContainer = container.Resolve<SetterInjection.CruiseTravel>();
```
注册是Register开头，获取是Resole开头。
step6:调用实例的具体方法
```
Console.WriteLine(domesticContainer.GetLineList()); 
Console.WriteLine(outboundContainer.GetLineList()); 
Console.WriteLine(cruiseContainer.GetLineList()); 
```
这是一个特别特别简单的Autofac的demo，通过这个小demo我可以简单的掌握Autofac的工作原理，但更多关于Autofac的细节还需要我今天更多更深入的了解才行。这篇文章就不展开了，以后有机会可以单独成文，另行介绍。

好了，这篇文章就到这里了，就到这里。

参考文章：

[1] 张洋,[依赖注入那些事儿](http://www.cnblogs.com/leoo2sk/archive/2009/06/17/di-and-ioc.html)

[2] 刘皓,[深入理解DIP、IoC、DI以及IoC容器](http://www.cnblogs.com/liuhaorain/p/3747470.html)

[3] Martin Fowler, [Inversion of Control Containers and the Dependency Injection pattern](http://www.martinfowler.com/articles/injection.html) 译文看[这里](https://pan.baidu.com/s/1o8OMmAe)，密码: ubyz
