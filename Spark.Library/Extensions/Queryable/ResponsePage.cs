namespace Spark.Library.Extensions.Queryable
{
    public class ResponsePage<T>
    {

        public ResponsePage()
        {
            
        }
        public ResponsePage(T? data)
        {
            Data = data;
        }


        public T? Data { get; set; }
    }
}
