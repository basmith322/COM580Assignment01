namespace Assignment01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateInstructor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        attendanceStatus = c.String(nullable: false),
                        eventID = c.Int(nullable: false),
                        studentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningEvent", t => t.eventID)
                .ForeignKey("dbo.Student", t => t.studentID)
                .Index(t => t.eventID)
                .Index(t => t.studentID);
            
            CreateTable(
                "dbo.LearningEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        eventType = c.String(nullable: false),
                        eventDateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        eventDuration = c.Int(nullable: false),
                        moduleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.moduleID)
                .Index(t => t.moduleID);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleCode = c.String(nullable: false),
                        ModuleName = c.String(nullable: false),
                        ModuleLevel = c.Short(nullable: false),
                        InstructorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instructors", t => t.InstructorId)
                .Index(t => t.InstructorId);
            
            CreateTable(
                "dbo.Instructors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffNum = c.String(nullable: false),
                        StaffName = c.String(nullable: false),
                        Module_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.Module_Id)
                .Index(t => t.Module_Id);
            
            CreateTable(
                "dbo.Register",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        regDate = c.DateTime(nullable: false, storeType: "date"),
                        studentID = c.Int(nullable: false),
                        moduleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Student", t => t.studentID)
                .ForeignKey("dbo.Modules", t => t.moduleID)
                .Index(t => t.studentID)
                .Index(t => t.moduleID);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        studentForname = c.String(nullable: false),
                        studentSurname = c.String(nullable: false),
                        studentNumber = c.String(nullable: false),
                        studentEmail = c.String(nullable: false),
                        studentTelNum = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Register", "moduleID", "dbo.Modules");
            DropForeignKey("dbo.Register", "studentID", "dbo.Student");
            DropForeignKey("dbo.Attendance", "studentID", "dbo.Student");
            DropForeignKey("dbo.LearningEvent", "moduleID", "dbo.Modules");
            DropForeignKey("dbo.Instructors", "Module_Id", "dbo.Modules");
            DropForeignKey("dbo.Modules", "InstructorId", "dbo.Instructors");
            DropForeignKey("dbo.Attendance", "eventID", "dbo.LearningEvent");
            DropIndex("dbo.Register", new[] { "moduleID" });
            DropIndex("dbo.Register", new[] { "studentID" });
            DropIndex("dbo.Instructors", new[] { "Module_Id" });
            DropIndex("dbo.Modules", new[] { "InstructorId" });
            DropIndex("dbo.LearningEvent", new[] { "moduleID" });
            DropIndex("dbo.Attendance", new[] { "studentID" });
            DropIndex("dbo.Attendance", new[] { "eventID" });
            DropTable("dbo.Student");
            DropTable("dbo.Register");
            DropTable("dbo.Instructors");
            DropTable("dbo.Modules");
            DropTable("dbo.LearningEvent");
            DropTable("dbo.Attendance");
        }
    }
}
