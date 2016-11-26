namespace msmq_base_support
{
    /* iBlockingReadonlyQueue<T> - a blocking, read-only queue interface */
    public interface iBlockingReadonlyQueue<T>
    {
        T Take();
        void start();
    }

}
