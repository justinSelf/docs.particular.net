---
title: ServiceControl Capacity Planning
summary: Details the ServiceControl capacity, throughput, and storage considerations to plan and support production environments
tags:
- ServiceControl
---

ServiceControl is a monitoring tool for production environments. As with other production monitoring tools, you must plan for and maintain the deployment over time.

The primary job of ServiceControl is to collect information on system behavior in production. It does so by collecting error, audit and health messages from dedicated queues. ServiceControl reads the messages flowing into those queues and stores them in its embedded database. In a production environment (and to a lesser degree in development, staging and testing environments), ServiceControl has an impact on the disk space where its data is stored, and its throughput capacity needs to be considered with regard to the overall system load and throughput.

### Storage

#### Location

Each ServiceControl instance stores its data in a RavenDB embedded instance. The location of the database has a significant impact on the overall system behavior in terms of performance and throughput. You should configure the embedded database files in a high-performance storage device that is connected to the ServiceControl machine with a high-throughput connection.

#### Size

The storage size that ServiceControl requires depends on the production load and is directly related to the quantity and size of messages that flow into the system.

Since ServiceControl is intended to be a recent-history storage to support ServicePulse and ServiceInsight monitoring and debugging activity. This is different from a long-term data archiving system, that is intended to provide extremely long term archiving and storage solutions (measured in years, subject to various business or regulatory requirements).

ServiceControl is configured with a default expiration policy that deletes old messages after a predefined time. The expiration policy can be customized to decrease or increase the amount of time data is retained, which impacts the storage requirements of ServiceControl.

To limit the rate at which the database grows the body of an audit messages can be truncated if it exceeds a configurable threshold. 

Refer to Data Retention section of [Customizing ServiceControl Configuration](creating-config-file.md) for details on these settings.


**NOTE**

* The maximum supported size of the RavenDB embedded database is 16TB.
* Failed messages are *never* expired and are retained indefinitely in the ServiceControl database.

### Accessing data and audited messages

#### Alternate Audit and Error queues

You can configure ServiceControl to forward any consumed messages into alternate queues, so that a copy of any message consumed by ServiceControl is available from these alternate queues.

For more details, see [Forwarding Queues](errorlog-auditlog-behavior.md)

#### Query the ServiceControl HTTP API

This provides a JSON stream of audited and error messages (headers, body, and context) that can be imported into another database.

NOTE: ServiceControl HTTP API is subject to changes and enhancements that may not be fully backwards compatible. Use of this HTTP API is discouraged by 3rd parties at this time.

### Throughput

ServiceControl consumes audited, error and control messages in its database. It does so for all the endpoints that are configured to forward these messages to the queues monitored by ServiceControl. This means that the throughput (measured in received and processed messages per second) required by ServiceControl is the aggregate throughput of all the endpoints forwarding messages to its queues.

The throughput of ServiceControl is dependent on multiple factors. Messages size and network bandwidth have significant affect on throughput. Another factor is the transport type used by your system.

#### Transport type

Different transports provide different throughput capabilities.

The transports supported by ServiceControl out-of-the-box (i.e. MSMQ, RabbitMQ, SQL Server and Azure Queues and Azure Service Bus) provide varying throughput numbers, with MSMQ and SQL Server providing the highest throughput numbers.

Azure Queues and Service Bus throughput varies significantly based on deployment options and multiple related variables inherent to cloud deployment scenarios.

It is recommended that you plan and perform realistic throughput tests on ServiceControl using the transport of your choice and deployment options that are as close as possible to your planned production deployment. For additional questions or information please [contact Particular Software](http://particular.net/contactus).