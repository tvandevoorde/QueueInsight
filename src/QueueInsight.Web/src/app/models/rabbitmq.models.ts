export interface VirtualHost {
  name: string;
}

export interface Queue {
  name: string;
  vhost: string;
  messages: number;
  messagesReady: number;
  messagesUnacknowledged: number;
  state: string;
}

export interface Message {
  payload: string;
  payloadEncoding: string;
  properties?: { [key: string]: any };
  routingKey?: string;
  payloadBytes: number;
  redelivered: boolean;
  exchange?: string;
}

export interface MessageResponse {
  messages: Message[];
  messageCount: number;
}

export interface PublishMessageRequest {
  vhost: string;
  queue: string;
  payload: string;
  payloadEncoding: string;
  properties?: { [key: string]: any };
}

export interface DeleteMessageRequest {
  vhost: string;
  queue: string;
  count: number;
}

export interface MoveMessageRequest {
  sourceVhost: string;
  sourceQueue: string;
  destinationVhost: string;
  destinationQueue: string;
  count: number;
}
