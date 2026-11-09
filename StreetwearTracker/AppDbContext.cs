using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StreetwearTracker

    // SQL is the file on my harddrive that store data so when we save data it will be there.

    //EF Core is a tool that let you write normal code like filters (.Where()) and auto translate into database language behind the scenes
{
    // internal means this class can be used only in streetwear tracker
    //Appdbcontext is just the name of the database bridge file
    internal class AppDbContext : DbContext // Inheritate from microsoft build in database management tools
    {
        // DbSet<> a special entity framework that represent a database table, like a spreadsheet
        public DbSet<ResaleItem> Items { get; set; } // hey track my resale items as a table
        
        // items is just the name of the table inside the database file 


        // Method explanation:

        
        // override - microsoft db context has a blank version of this config method, we just taking it and filling it with our own rules.

        //Onconfirguring is a normal microsoft method.
        
        // dbcontextoptionsbuilder optionsBuilder is the toolbag passed into the method, so it gives you tools you need to plug in your database settings.


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
           

            // telling EF Core is our tool that let us write code to make filters like EF Core to use sqlite as our db type

            // datasource = inventory.db is the file name on your harddrive, if inventory.db dont exist ef core will make it
            
            
            optionsBuilder.UseSqlite("Data Source=inventory.db"); // permanent file saved on the harddrive
        }
    }
}
