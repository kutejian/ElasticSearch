using Nest;

namespace WebApplication1.Api
{
    public class Config: IConfig
    {
        public IConfiguration _Configuration;
        public IElasticClient client;
        public Config(IConfiguration configuration,IElasticClient elasticClient)
        {
            _Configuration = configuration;
            client = elasticClient;
        }
        public void ww()
        {
            //var a = _Configuration["URL"];
            string indexName = "user";
            var user = new User()
            {
                Id = 1,
                Age = 189,
                Name = "MicroHeart",
                Gender = true
            };
            var user2 = new User()
            {
                Id = 2,
                Age = 13,
                Name = "kutejiang",
                Gender = true
            };

            //检查是否连接成功
            var infoResponse = client.RootNodeInfo();
            //创建索引
            var createIndexResponse = client.Indices.Create(
                indexName, c => c.Map<User>(m => m.AutoMap()));
            
            //添加文档数据
            var resp1 = client.Index(user, s => s.Index(indexName));//指定操作索引名称
            //批量添加文档数据
            var bulkResponse = client.Bulk(b => b    
                           .Index("my-index")
                           .IndexMany(new List<User>() { user, user2 }));


            //删除索引
            var deleteIndexResponse = client.Indices.Delete("my-index");
            //单个删除
            var resp3 = client.Delete<User>(1, d => d.Index(indexName));
            //删除年龄小于18的
            var resp4 = client.DeleteByQuery<User>(x => x.Index(indexName)
            .Query(q => q.Range(r => r.Field(f => f.Age).LessThan(18))));

            //查询文档数据
            var resp5 = client.Get<User>(1, d => d.Index(indexName));
            //根据Id集合获取多个
            var resp6 = client.GetMany<User>(new List<long>() { 19L, 20L, 21L }, indexName);
            //根据条件查询
            var resp7 = client.Search<User>(x => x.Index(indexName)
                                     .From(0)
                                     .Size(20)
                                     .Query(q => q.Range(r => r.Field(f => f.Age).GreaterThan(18)) &&
                                                 q.Range(r => r.Field(f => f.Age).LessThan(30)) &&
                                                 q.Term(t => t.Gender, true)));

            //修改文档 更新
            var user3 = new User()
            {
                Age = 18,
                Gender = false,
                Name = "test"
            };
            var resp8 = client.Update<User>(
                20, u => u.Index(indexName).Doc(user));
            //修改索引 更新
            var updateSettingsResponse = client.Indices.UpdateSettings("your-index-name", u => u
                .IndexSettings(s => s
                    .NumberOfReplicas(1)
                    ));
        }
    }
}
