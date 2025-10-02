import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ConnectionConfigComponent } from './components/connection-config/connection-config.component';
import { VhostListComponent } from './components/vhost-list/vhost-list.component';
import { QueueListComponent } from './components/queue-list/queue-list.component';
import { MessageViewerComponent } from './components/message-viewer/message-viewer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet, 
    CommonModule,
    ConnectionConfigComponent,
    VhostListComponent,
    QueueListComponent,
    MessageViewerComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'QueueInsight';
  selectedVhost: string | null = null;
  selectedQueue: string | null = null;

  onVhostSelected(vhost: string) {
    this.selectedVhost = vhost;
    this.selectedQueue = null;
  }

  onQueueSelected(queue: string) {
    this.selectedQueue = queue;
  }
}
