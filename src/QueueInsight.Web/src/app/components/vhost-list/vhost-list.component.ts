import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RabbitmqService } from '../../services/rabbitmq.service';
import { VirtualHost } from '../../models/rabbitmq.models';

@Component({
  selector: 'app-vhost-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './vhost-list.component.html',
  styleUrl: './vhost-list.component.scss'
})
export class VhostListComponent implements OnInit {
  @Output() vhostSelected = new EventEmitter<string>();
  
  vhosts: VirtualHost[] = [];
  selectedVhost: string | null = null;
  loading = false;
  error: string | null = null;

  constructor(private rabbitmqService: RabbitmqService) {}

  ngOnInit() {
    this.loadVhosts();
  }

  loadVhosts() {
    this.loading = true;
    this.error = null;
    
    this.rabbitmqService.getVirtualHosts().subscribe({
      next: (vhosts) => {
        this.vhosts = vhosts;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load virtual hosts';
        this.loading = false;
        console.error(err);
      }
    });
  }

  selectVhost(vhost: string) {
    this.selectedVhost = vhost;
    this.vhostSelected.emit(vhost);
  }

  refresh() {
    this.loadVhosts();
  }
}

