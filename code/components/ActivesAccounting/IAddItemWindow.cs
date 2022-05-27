namespace ActivesAccounting;

internal interface IAddItemWindow<T>
{
    bool ShowWindow(out T? aItem, out int? aIndex);
}