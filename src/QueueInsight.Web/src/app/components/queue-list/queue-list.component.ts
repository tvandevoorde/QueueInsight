import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RabbitmqService } from '../../services/rabbitmq.service';
import { Queue } from '../../models/rabbitmq.models';

@Component({
  selector: 'app-queue-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './queue-list.component.html',
  styleUrl: './queue-list.component.scss'
})
export class QueueListComponent implements OnChanges {
  @Input() vhost!: string;
  @Output() queueSelected = new EventEmitter<string>();
  
  queues: Queue[] = [];
  selectedQueue: string | null = null;
  loading = false;
  error: string | null = null;

  constructor(private rabbitmqService: RabbitmqService) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['vhost'] && this.vhost) {
      this.loadQueues();
    }
  }

  loadQueues() {
    this.loading = true;
    this.error = null;
    
    this.rabbitmqService.getQueues(this.vhost).subscribe({
      next: (queues) => {
        this.queues = queues;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load queues';
        this.loading = false;
        console.error(err);
      }
    });
  }

  selectQueue(queue: string) {
    this.selectedQueue = queue;
    this.queueSelected.emit(queue);
  }

  refresh() {
    this.loadQueues();
  }
}
