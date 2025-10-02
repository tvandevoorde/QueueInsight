import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RabbitmqService } from '../../services/rabbitmq.service';
import { Message, PublishMessageRequest, DeleteMessageRequest, MoveMessageRequest } from '../../models/rabbitmq.models';

@Component({
  selector: 'app-message-viewer',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './message-viewer.component.html',
  styleUrl: './message-viewer.component.scss'
})
export class MessageViewerComponent implements OnChanges {
  @Input() vhost!: string;
  @Input() queue!: string;
  
  messages: Message[] = [];
  messageCount = 10;
  loading = false;
  error: string | null = null;
  selectedMessage: Message | null = null;
  
  // For publishing messages
  showPublishForm = false;
  newMessagePayload = '';
  
  // For moving messages
  showMoveForm = false;
  moveToVhost = '';
  moveToQueue = '';
  moveCount = 1;
  
  // For deleting messages
  deleteCount = 1;

  constructor(private rabbitmqService: RabbitmqService) {}

  ngOnChanges(changes: SimpleChanges) {
    if ((changes['vhost'] || changes['queue']) && this.vhost && this.queue) {
      this.loadMessages();
    }
  }

  loadMessages() {
    this.loading = true;
    this.error = null;
    
    this.rabbitmqService.getMessages(this.vhost, this.queue, this.messageCount).subscribe({
      next: (response) => {
        this.messages = response.messages;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load messages';
        this.loading = false;
        console.error(err);
      }
    });
  }

  selectMessage(message: Message) {
    this.selectedMessage = message;
  }

  refresh() {
    this.loadMessages();
  }

  togglePublishForm() {
    this.showPublishForm = !this.showPublishForm;
    this.showMoveForm = false;
  }

  toggleMoveForm() {
    this.showMoveForm = !this.showMoveForm;
    this.showPublishForm = false;
    this.moveToVhost = this.vhost;
  }

  publishMessage() {
    if (!this.newMessagePayload.trim()) {
      alert('Please enter a message payload');
      return;
    }

    const request: PublishMessageRequest = {
      vhost: this.vhost,
      queue: this.queue,
      payload: this.newMessagePayload,
      payloadEncoding: 'string'
    };

    this.rabbitmqService.publishMessage(request).subscribe({
      next: () => {
        alert('Message published successfully');
        this.newMessagePayload = '';
        this.showPublishForm = false;
        this.loadMessages();
      },
      error: (err) => {
        alert('Failed to publish message: ' + err.message);
        console.error(err);
      }
    });
  }

  deleteMessages() {
    if (this.deleteCount < 1) {
      alert('Delete count must be at least 1');
      return;
    }

    if (!confirm(`Are you sure you want to delete ${this.deleteCount} message(s)?`)) {
      return;
    }

    const request: DeleteMessageRequest = {
      vhost: this.vhost,
      queue: this.queue,
      count: this.deleteCount
    };

    this.rabbitmqService.deleteMessages(request).subscribe({
      next: () => {
        alert('Messages deleted successfully');
        this.loadMessages();
      },
      error: (err) => {
        alert('Failed to delete messages: ' + err.message);
        console.error(err);
      }
    });
  }

  moveMessages() {
    if (!this.moveToVhost.trim() || !this.moveToQueue.trim()) {
      alert('Please specify destination vhost and queue');
      return;
    }

    if (this.moveCount < 1) {
      alert('Move count must be at least 1');
      return;
    }

    const request: MoveMessageRequest = {
      sourceVhost: this.vhost,
      sourceQueue: this.queue,
      destinationVhost: this.moveToVhost,
      destinationQueue: this.moveToQueue,
      count: this.moveCount
    };

    this.rabbitmqService.moveMessages(request).subscribe({
      next: () => {
        alert('Messages moved successfully');
        this.showMoveForm = false;
        this.loadMessages();
      },
      error: (err) => {
        alert('Failed to move messages: ' + err.message);
        console.error(err);
      }
    });
  }
}
