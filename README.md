# gRPC Practice
> Order API to train on using gRPC in microservices. This API comminucates with two services (Payment, Inventory) in an Ordering API solution. The project helped me learn how to define services with Protocol Buffers and set up gRPC communication between different parts of a system.

[![](http://img.shields.io/badge/framework-.NetCore-blue.svg?style=flat)](https://maven.apache.org/)
[![](http://img.shields.io/badge/language-CShap-brightgreen.svg?color=darkgreen)](https://www.oracle.com/java/technologies/downloads/)
![](https://img.shields.io/github/last-commit/kareem983/GRPC-Practice)

# About
- MakeOrder endpoint is the api that comminucate with two main services using gRPC.
- Payment service checks that the user authorized to make the order or not.
- Inventory service checks that the items in the order are availble or not.
- The MakeOrder endpoint returns the status of each service and the status of order overall.

# Prerequisites
- [1] Install Visual Studio (recommended 2022).
- [2] Install .Net Core 8.
- [3] Install gRPC.ASP.NetCore Package.
- [4] Install gRPC.ASP.NetCore.Reflection Package.

# References/Resources

- [.Net Guide](https://visualstudio.microsoft.com/vs/features/net-development/)
  
- [gRPC Documentation](https://grpc.io/docs/)
  
- [Protocol Buffer Guide](https://protobuf.dev/overview/)
  
  
