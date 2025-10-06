using System;
using LibraryApp.Domain;
using LibraryApp.Domain.Enums;
using LibraryApp.Domain.Entities;
using LibraryItem = LibraryApp.Domain.Entities.LibraryItem;

namespace LibraryApp.Application.Mappers
{
    public static class LibraryItemMapper
    {
        public static Domain.LibraryItem ToDomain(this LibraryItem entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            return (LibraryItemTypeEnum)entity.Type switch
            {
                LibraryItemTypeEnum.Book => new Book(entity.Id, entity.Title ?? string.Empty, entity.Author ?? string.Empty, entity.Pages ?? 0),
                LibraryItemTypeEnum.Magazine => new Magazine(entity.Id, entity.Title ?? string.Empty, entity.IssueNumber ?? 0, entity.Publisher ?? string.Empty),
                _ => throw new InvalidOperationException("Unknown library item type.")
            };
        }
    }
}