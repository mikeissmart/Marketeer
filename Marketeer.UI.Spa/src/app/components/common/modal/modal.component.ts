import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import {
  NgbModalRef,
  NgbModal,
  NgbModalOptions,
} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
})
export class ModalComponent implements OnInit {
  _isOpen = false;
  @Input() set isOpen(value: boolean) {
    if (value != this._isOpen) {
      if (value) {
        this.openModal();
      } else {
        this.closeModal();
      }
    }
  }
  @Input()
  showCloseBtn = true;
  @Input()
  isStatic = true;
  @Input()
  size: 'sm' | 'md' | 'lg' | 'xl' = 'lg';
  @Input()
  fullscreen: 'sm' | 'md' | 'lg' | 'xl' | 'xxl' | boolean | null = null;
  @Output()
  onClose = new EventEmitter();

  @ViewChild('modal', { static: false })
  private modal: TemplateRef<ModalComponent> | null = null;

  modalRef: NgbModalRef | null = null;

  constructor(private readonly modalService: NgbModal) {}

  ngOnInit(): void {}

  public openModal(): void {
    this.modalService.dismissAll();
    this.modalRef = this.modalService.open(this.modal, {
      animation: true,
      backdrop: this.isStatic ? 'static' : '',
      centered: true,
      keyboard: true,
      scrollable: true,
      size: this.size,
      fullscreen: this.fullscreen,
    } as NgbModalOptions);
    this._isOpen = true;
  }

  closeModal(): void {
    this._isOpen = false;
    this.modalService.dismissAll();
    this.onClose.emit();
  }
}
