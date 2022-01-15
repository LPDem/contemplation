@echo off
docker run -d --name contemplation -p:8080:80 -e "ImagesFolder=/images" -v D:\Temp\Images\:/images local/contemplation