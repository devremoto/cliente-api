import { Component, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { Cliente } from './models/cliente';
import { ClienteService } from './services/cliente.service';
import { ToasterService } from './services/toaster.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Web';
  cliente: Cliente = {};
  clientes: Observable<Cliente[]> = new Observable<Cliente[]>();
  ref!: NgbModalRef;
  closeResult = '';
  loading = false;
  @ViewChild('content') content: any;

  constructor(
    private clienteService: ClienteService,
    private toasterService: ToasterService,
    private modalService: NgbModal,
  ) {
    this.load();
  }

  load(): void {
    this.clientes = this.clienteService.get();
  }

  save(): void {
    if (!this.validate()) {
      return;
    }
    this.clienteService.save(this.cliente).subscribe(
      _ => {
        const msg = this.cliente.id ? 'Cliente Alterado com sucesso' : 'Cliente cadastrado com sucesso';
        this.toasterService.pop('success', msg);
        this.closePopup('');
        this.load();
      },
      _ => {
        const msg = this.cliente.id ? 'Erro ao alterar o Cliente' : 'Erro ao cadastrar o Cliente';
        this.toasterService.pop('error', msg);
      }
    );
  }

  validate(): boolean {

    let error = '';
    if (!this.cliente.nome) {
      error = 'O campo nome é obrigatório';
    }
    if (!this.cliente.idade) {
      error = 'O campo idade é obrigatório';
    }

    if (error) {
      this.toasterService.error(error);
      return false;
    }
    return true;
  }

  new(): void {
    this.cliente = new Cliente();
    this.openPopup(this.content);
  }
  edit(cliente: Cliente): void {
    this.cliente = cliente;
    this.openPopup(this.content);
  }

  remove(cliente: Cliente): void {
    this.cliente = cliente;
    this.clienteService.delete(this.cliente).subscribe(
      _ => {
        this.toasterService.pop('success', 'Cliente Excluido com sucesso');
        this.closePopup('');
        this.load();
      },
      _ => {
        this.toasterService.pop('error', 'Erro ao excluir o Cliente');
      }
    );
  }

  openPopup(content: any): void {
    this.ref = this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title', size: 'lg' });

    this.ref.result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.closePopup(reason)}`;
    });
  }

  private closePopup(reason?: any): void {
    this.ref.dismiss(reason);
  }
}


