using DbHelper;
using System;
using System.Threading.Tasks;

namespace DbManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var query = "select * from users" ;//where Id = @Id and FullName=@FullName";

            var users = await new DbAccessHelper().ReadDataAsync<User>(query);

            foreach(var user in users)
            {
                Console.WriteLine(user.Id);
                Console.WriteLine(user.FullName);
                Console.WriteLine("===================================");
            }

            //var insertQuery = @"Insert into Users(FullName) values(@FullName)";
            //var rowsAffected = await new DbAccessHelper().InsertDataAsync(insertQuery, new { FullName="Jack"});


            //var insertQuery = @"Insert into Users(FullName) values(@FullName) select Scope_Identity()";
            //var rowsAffected = await new DbAccessHelper().GetScalarAsync<int>(insertQuery, new { FullName = "Matt" });

            //Console.WriteLine(rowsAffected);

            //var updateQuery = @"Update users set FullName=@FullName where Id=@Id";
            //var rowsAffected = await new DbAccessHelper().UpdateAsync(updateQuery, new { FullName = "John Sir", Id = 4 });
            //Console.WriteLine(rowsAffected);

            //var deleteQuery = @"delete from users where FullName=@FullName";
            //var rowsAffected = await new DbAccessHelper().DeleteAsync(deleteQuery, new { FullName = "Matt Sir" });
            //Console.WriteLine(rowsAffected);

            Console.ReadLine();
        }
    }
}
