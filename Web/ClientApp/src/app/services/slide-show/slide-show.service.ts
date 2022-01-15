import { Injectable } from '@angular/core';
import { filter } from 'rxjs';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { ImagesBackendService } from '../../backend/images-backend-service';

@Injectable()
export class SlideShowService {

  constructor(
    private imagesBackendService: ImagesBackendService
  ) { }

  private _imageListSubject: BehaviorSubject<string[]> = new BehaviorSubject<string[]>(null);

  private _imageList: string[] = null;

  private _currentIndex: number = 0;

  getList(): Observable<string[]> {
    if (!this._imageList) {
      this.updateList();
    }

    return this._imageListSubject.pipe(
      filter(t => !!t)
    );
  }

  updateList(): void {
    this.loadImageList();
  }

  getCurrentIndex(): number {
    return this._currentIndex;
  }

  setCurrentIndex(value: number): void {
    this._currentIndex = value;
  }

  private loadImageList() {
    this.imagesBackendService.getAll().subscribe(data => {
      data = this.shuffle(data);
      this._imageListSubject.next(data);
    });
  }

  private shuffle(data: string[]): string[] {
    return data.map((value) => ({ value, sort: Math.random() }))
      .sort((a, b) => a.sort - b.sort)
      .map(({ value }) => value);
  }

}
