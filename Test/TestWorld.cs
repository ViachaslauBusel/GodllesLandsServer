using NetworkGameEngine;

namespace Test
{
    public class TestComponent : Component
    {
    public volatile int m_init = 0;
        public volatile int m_awake = 0;
        public volatile int m_start = 0;
        public volatile int m_update = 0;
        public volatile int m_destroy = 0;
        public override async Task Init()
        {
            m_init++;
            Console.WriteLine($"Hello from Init:{Thread.CurrentThread.ManagedThreadId}");
        }

        public override void Awake()
        {
        m_awake++;
            Console.WriteLine($"Hello from Awake:{Thread.CurrentThread.ManagedThreadId}");
        }

        public override void Start()
        {
        m_start++;
            Console.WriteLine($"Hello from Start:{Thread.CurrentThread.ManagedThreadId}");
        }

        public override void Update()
        {
        m_update++;
            Console.WriteLine($"Hello from Update:{Thread.CurrentThread.ManagedThreadId}");
        }

        public override void OnDestroy()
        {
            m_destroy++;
            Console.WriteLine($"Hello from OnDestroy:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
    public class TestWorld
    {
        [SetUp]
        public void Setup()
        {
            World.Init(8);
        }

        [Test]
        public async Task Test1()
        {
            GameObject testObj = new GameObject();
            TestComponent testComponent = new TestComponent();
            testObj.AddComponent(testComponent);  

            int id = await World.AddGameObject(testObj);

            Thread.Sleep(1_000);
            World.RemoveGameObject(id);
            Thread.Sleep(1_000);
            Assert.IsTrue(testComponent.m_init == 1);
            Assert.IsTrue(testComponent.m_awake == 1);
            Assert.IsTrue(testComponent.m_start== 1);
            Assert.IsTrue(testComponent.m_destroy == 1);

            Console.WriteLine($"Update:{testComponent.m_update}");
            Assert.Pass();
        }
    }
}