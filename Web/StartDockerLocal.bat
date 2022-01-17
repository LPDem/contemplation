@echo off
docker run -d --name contemplation -p:10000:80 -v D:\Temp\Images\:/images local/contemplation