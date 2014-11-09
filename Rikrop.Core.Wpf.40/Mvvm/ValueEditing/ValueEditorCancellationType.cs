namespace Rikrop.Core.Wpf.Mvvm.ValueEditing
{
    public enum ValueEditorCancellationType
    {
        /// <summary>
        /// вызвана пользователем
        /// </summary>
        Manual = 0,

        /// <summary>
        /// отмена редактирования, т.к. в процессе сохранения произошла ошибка
        /// </summary>
        OnSaveError = 1,

        /// <summary>
        /// отмена редактирования, т.к. сохраняемое значение не удовлетворяет правилам валидации
        /// </summary>
        OnSaveValidationRollback = 2,
    }
}