# ZookeeperWinServiceHost

A tool used to host zookeeper server instance as windows service

## how to use ?

1. download zookeeper binary and unzip(http://archive.apache.org/dist/zookeeper/)
2. download Release folder of ZookeeperWinServiceHost repository
3. copy all items under Release folder to bin directory of zookeeper
4. set Appsetting.config,
  only three config item need to be set:
  
   ServiceName: indicate service name, if you need deplay cluster environment, you must ensure every node name are not same  
   ServiceDesc: you can give any description for your windows service  
   ServiceDisplayName: service display name 
  
5. execute Install.bat as administrator permission



   


## what's the advantages over prunsrv ?

the ZookeeperWinServiceHost can kill java.exe when stop windows service ,but prunsrv can not.

prunsrv refer link:(http://archive.apache.org/dist/commons/daemon/binaries/windows/)
