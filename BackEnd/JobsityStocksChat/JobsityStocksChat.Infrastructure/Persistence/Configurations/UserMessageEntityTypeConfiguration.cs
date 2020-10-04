using JobsityStocksChat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityStocksChat.Infrastructure.Persistence.Configurations
{
    public class UserMessageEntityTypeConfiguration : IEntityTypeConfiguration<UserMessage>
    {
        public void Configure(EntityTypeBuilder<UserMessage> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasDefaultValueSql("newid()");

            builder.HasOne(t => t.User)
                .WithMany(t => t.Messages)
                .HasForeignKey(t => t.UserId);
        }
    }
}
