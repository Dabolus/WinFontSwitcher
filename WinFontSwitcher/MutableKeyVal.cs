namespace WinFontSwitcher
{
    public class MutableKeyVal<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public MutableKeyVal()
        {
        }

        public MutableKeyVal(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}