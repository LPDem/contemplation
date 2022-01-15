import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ImagesBackendService } from './backend/images-backend-service';
import { SlideShowComponent } from './components/slide-show/slide-show.component';
import { SlideShowService } from './services/slide-show/slide-show.service';
import { SwipeDirective } from './directives/swipe/swipe.directive';

@NgModule({
  declarations: [
    AppComponent,
    SlideShowComponent,
    SwipeDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    ImagesBackendService,
    SlideShowService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
