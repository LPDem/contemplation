import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SlideShowComponent } from './components/slide-show/slide-show.component';

const routes: Routes = [
  {
    path: '',
    component: SlideShowComponent
  },
  {
    path: ':filename',
    component: SlideShowComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
