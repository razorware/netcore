It is extremely important that anyone working on the Data namespace understands that performace is desirable over readability.
Please comment generously. Some depth of tribal knowledge is permissable.

Update this readme with notes on features, bug fixes, etc.

# NOTES:
## _Data_ Namespace - 

* DataExtensions.cs - 
  Implementing a set of extensions to build and configure a schema. The delegates are extended to create a single fluent API delegate.
  This means that a fluent delegate can be further extended prior to execution and execute immediate, lazy or a mix.

  Prior to working on `DataExtensions`, you should have a deep understanding of delegates. Take time to understand what the `DataExtensions`
  API is doing and how it behaves. Seemingly minor changes in the behavior of a delegate can have extensive ramifications throughout the 
  execution of the delegate.

  Benefits of this particular fluent API paradigm (delegate extensions) include the ease of threading and possibility of atomic execution.