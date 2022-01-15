import { DOCUMENT } from '@angular/common';
import { AfterViewInit, Component, ElementRef, HostListener, Inject, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { combineLatest, filter, map, startWith, tap } from 'rxjs';
import { ImagesBackendService } from '../../backend/images-backend-service';
import { SlideShowService } from '../../services/slide-show/slide-show.service';

@Component({
  selector: 'app-slide-show',
  templateUrl: './slide-show.component.html',
  styleUrls: ['./slide-show.component.scss']
})
export class SlideShowComponent implements OnInit, AfterViewInit {

  data: string[] = null;
  image: string = null;
  imageUrl: string = null;
  imageBackUrl: string = null;
  imageIndex: number = 0;
  isControlsHidden: boolean = true;
  isImageLoading: boolean = false;
  isTap: boolean = true;
  isFullscreen: boolean = false;

  @ViewChild('container') container: ElementRef;

  constructor(
    private imagesBackendService: ImagesBackendService,
    private slideShowService: SlideShowService,
    @Inject(DOCUMENT) private document: Document,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  ngAfterViewInit(): void {

    var dataPipe = this.slideShowService
      .getList()
      .pipe(
        tap(result => {
          this.data = result;
          this.startChangeImageTimer();
        })
      );

    var routePipe = this.route.paramMap
      .pipe(
        filter(t => t.has('filename')),
        map(t => t.get('filename')),
        startWith(null)
      );

    combineLatest({ data: dataPipe, filename: routePipe }).subscribe(result => {
      if (result.filename) {
        this.findImageIndexByFilename(result.filename);
      }

      this.loadCurrentImage();
    });
  }

  private _changeImageTimer: any = null;

  private _hideControlsTimer: any = null;

  ngOnInit(): void {
  }

  onNextButtonClick(event: Event): void {
    event.stopPropagation();
    this.next();
  }

  onPrevButtonClick(event: Event): void {
    event.stopPropagation();
    this.prev();
  }

  onStopButtonClick(event: Event): void {
    event.stopPropagation();
    this.stop();
  }

  onStartButtonClick(event: Event): void {
    event.stopPropagation();
    this.start();
  }

  onRefreshButtonClick(event: Event): void {
    event.stopPropagation();
    this.slideShowService.updateList();
  }

  isTimerActive(): boolean {
    return !!this._changeImageTimer;
  }

  onContainerMouseMove(): void {
    if (this.isTap) {
      this.isTap = false;
      return;
    }
    this.showControls();
  }

  onContainerClick(): void {
    this.toggleControls();
    this.isTap = true;
  }

  @HostListener('document:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    switch (event.code) {
      case 'ArrowLeft':
        this.prev();
        this.showControls();
        break;
      case 'ArrowRight':
        this.next();
        this.showControls();
        break;
      case 'Space':
        this.togglePlayMode();
        this.showControls();
        break;
      case 'KeyF':
      case 'Enter':
        this.toggleFullscreen();
        break;
      //  case 'Escape':
      //    this.toggleControls();
      //    break;
    }
  }

  onImageLoad(): void {
    this.isImageLoading = false;
  }

  onSwipeLeft(): void {
    this.prev();
  }

  onSwipeRight(): void {
    this.next();
  }

  onEnterFullScreenButtonClick(event: Event): void {
    event.stopPropagation();
    this.enterFullscreen();
  }

  onExitFullScreenButtonClick(event: Event): void {
    event.stopPropagation();
    this.exitFullscreen();
  }

  @HostListener('document:fullscreenchange', ['$event'])
  onFullscreenChange(event: Event): void {
    this.isFullscreen = !!this.document.fullscreenElement;
    this.loadCurrentImage();
  }

  private loadCurrentImage(): void {
    this.imageIndex = this.slideShowService.getCurrentIndex();
    this.image = this.data[this.imageIndex];
    var containerElement = this.container.nativeElement;
    this.imageBackUrl = this.imageUrl;
    this.imageUrl = this.imagesBackendService.getFullImageUrl(this.image, containerElement.clientWidth, containerElement.clientHeight);
    this.isImageLoading = true;

    var url = this.router.createUrlTree([this.image], { relativeTo: this.route.parent }).toString();
    window.history.replaceState({}, '', url);
  }

  private next(): void {
    this.loadNextImage();
    this.stopChangeImageTimer();
  }

  private prev(): void {
    this.loadPrevImage();
    this.stopChangeImageTimer();
  }

  private stop(): void {
    this.stopChangeImageTimer();
  }

  private start(): void {
    this.startChangeImageTimer();
  }

  private loadNextImage(): void {
    var index = this.slideShowService.getCurrentIndex();
    index++;

    if (index >= this.data.length) {
      index = 0;
    }

    this.slideShowService.setCurrentIndex(index);
    this.loadCurrentImage();
  }

  private loadPrevImage(): void {
    var index = this.slideShowService.getCurrentIndex();
    index--;

    if (index < 0) {
      index = this.data.length - 1;
    }

    this.slideShowService.setCurrentIndex(index);
    this.loadCurrentImage();
  }

  private startChangeImageTimer(): void {
    this.stopChangeImageTimer();
    this._changeImageTimer = setTimeout(this.onChangeImageTimer.bind(this), 5 * 1000);
  }

  private stopChangeImageTimer(): void {
    if (this._changeImageTimer) {
      clearTimeout(this._changeImageTimer);
      this._changeImageTimer = null;
    }
  }

  private onChangeImageTimer(): void {
    this.loadNextImage();
    this.startChangeImageTimer();
  }

  private showControls(): void {
    this.isControlsHidden = false;
    this.restartHideControlsTimer();
  }

  private restartHideControlsTimer(): void {
    if (this._hideControlsTimer) {
      clearTimeout(this._hideControlsTimer);
    }
    this._hideControlsTimer = setTimeout(() => {
      this.isControlsHidden = true;
    }, 3 * 1000);
  }

  private toggleControls(): void {
    if (this.isControlsHidden) {
      this.showControls();
    } else {
      this.isControlsHidden = true;
    }
  }

  private togglePlayMode(): void {
    if (this.isTimerActive()) {
      this.stop();
    } else {
      this.start();
    }
  }

  private enterFullscreen(): void {
    this.container.nativeElement.requestFullscreen();
  }

  private exitFullscreen(): void {
    this.document.exitFullscreen();
  }

  private toggleFullscreen(): void {
    if (this.isFullscreen) {
      this.exitFullscreen();
    } else {
      this.enterFullscreen();
    }
  }

  private findImageIndexByFilename(filename: string): void {
    var index = this.data.indexOf(filename);
    if (index >= 0) {
      this.slideShowService.setCurrentIndex(index);
    }
  }

}
