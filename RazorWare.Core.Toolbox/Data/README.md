It is extremely important that anyone working on the Data namespace understands that performace is desirable over readability.
Please comment generously. Some depth of tribal knowledge is permissable.

Update this readme with notes on features, bug fixes, etc.

# NOTES:
## _Data_ Namespace - 

* DataExtensions.cs - 
  Implementing a set of extensions to build and configure a schema. The delegates are extended to create a single fluent API delegate.
  This means that a fluent delegate can be further extended prior to execution and execution is essentially lazy. Performance *seems* to
  be improved over instanced classes and class instance extensions although no hard data has been generated to substantiate the same.