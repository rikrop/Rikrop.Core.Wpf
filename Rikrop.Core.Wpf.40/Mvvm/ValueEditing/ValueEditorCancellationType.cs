namespace Rikrop.Core.Wpf.Mvvm.ValueEditing
{
    public enum ValueEditorCancellationType
    {
        /// <summary>
        /// ������� �������������
        /// </summary>
        Manual = 0,

        /// <summary>
        /// ������ ��������������, �.�. � �������� ���������� ��������� ������
        /// </summary>
        OnSaveError = 1,

        /// <summary>
        /// ������ ��������������, �.�. ����������� �������� �� ������������� �������� ���������
        /// </summary>
        OnSaveValidationRollback = 2,
    }
}