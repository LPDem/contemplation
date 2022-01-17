# Contemplation slideshow
An image gallery, geared toward slideshow.

Only relaxed contemplation, no comments, likes and other hustle.

Images are shown in random order, changing after 5 sec. Images scaled proportionally to fit the screen. "Missings parts" are filled with enlarged and blurred image.
All image processing is done on the server size, no browser resampling.

Controls:
- Previous and next navigation, also with swipe
- Pause and resume
- Toggle fullscreen 

It's written on .Net Core and Angular, cross-platforming, can run on any webserver or standalone.

## Docker

Download, build and run docker image:

### Docker CLI

```
git clone https://github.com/LPDem/contemplation.git .
cd Web
docker build -t local/contemplation:latest .
docker run -d --name contemplation -p:10000:80 -v /your/images:/images local/contemplation
```

### Docker-compose

```
services:
  contemplation:
    container_name: contemplation
    image: local/contemplation
    build: ./Web
    restart: always
    volumes:
      - /your/images:/images
    ports:
      - 10000:80
```

Replace /your/images with your images folder.

## Regular server app

Download release from here: https://github.com/LPDem/contemplation/releases

Create file **appsettings.Production.json** near appsettings.json and specify path to your images folder:

```
{
  "ImagesFolder": "/your/images"
}
```

### Using web server
Run app in your web server (IIS, Nginx, etc) as usual for .Net Core web app.

### Running stadalone
Like any .Net Core web app, you can run it standalone using build-in Kestrel web server.

Add Kestrel configuration to **appsettings.Production.json**:

```
{
  "ImagesFolder": "/your/images",
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://*:10000"
      }
    }
  }
}
```

and run:

```
dotnet Contemplation.dll
```

## Plans
- Subfolders support
- Thumbnails browser
- Various sortings
- More settings in config
- Customizable colors
