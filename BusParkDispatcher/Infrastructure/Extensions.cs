using System.Data.Entity;

namespace BusParkDispatcher.Infrastructure
{
    static class Extensions
    {
        public static void UndoChanges(this DbContext dbContext)
        {
            // Метод-расширение UndoChanges()

            // Проходим по всем изменениям в базе данных
            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                switch (entry.State) // Выбираем состояние данного изменения
                {
                    case EntityState.Modified: // Если данные изменены, отмечаем как не измененные
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted: // Если данные удалены, загружаем их заново
                        entry.Reload();
                        break;
                    case EntityState.Added: // Если данные были добавлены, то убираем с них отслеживание
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }
    }
}
