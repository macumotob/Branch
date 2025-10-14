using _7E.Branch;
using _7E.Branch.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace BranchLike.Api;


public class AppDb : DbContext
{
    public DbSet<Link> Links => Set<Link>();
    public DbSet<LinkClick> LinkClicks => Set<LinkClick>();


    public AppDb(DbContextOptions<AppDb> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Link>(e =>
        //{
        //    e.ToTable("links");
        //    e.HasKey(x => x.Id);
        //    e.HasIndex(x => x.ShortCode).IsUnique();
        //    e.Property(x => x.Id).HasColumnName("id");
        //    e.Property(x => x.ShortCode).HasColumnName("short_code").IsRequired();
        //    e.Property(x => x.LongUrl).HasColumnName("long_url").IsRequired();
        //    e.Property(x => x.Title).HasColumnName("title");
        //    e.Property(x => x.Description).HasColumnName("description");
        //    e.Property(x => x.MetadataJson).HasColumnName("metadata_json");
        //    e.Property(x => x.ControlParamsJson).HasColumnName("control_params_json");
        //    e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        //    e.Property(x => x.click_count).HasColumnName("click_count").HasDefaultValue(0);
        //});


        //modelBuilder.Entity<LinkClick>(e =>
        //{
        //    e.ToTable("link_clicks");
        //    e.HasKey(x => x.Id);
        //    e.HasIndex(x => x.LinkId);
        //    e.Property(x => x.Id).HasColumnName("id");
        //    e.Property(x => x.LinkId).HasColumnName("link_id");
        //    e.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        //    e.Property(x => x.UserAgent).HasColumnName("user_agent");
        //    e.Property(x => x.Referer).HasColumnName("referer");
        //    e.Property(x => x.Ip).HasColumnName("ip");
        //    e.Property(x => x.Query).HasColumnName("query");


        //    e.HasOne<Link>()
        //    .WithMany()
        //    .HasForeignKey(x => x.LinkId)
        //    .OnDelete(DeleteBehavior.Cascade);
        //});
    }
}