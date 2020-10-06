import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { ChatMessage } from '../services/chat.service';

@Component({
  selector: 'app-chat-message-Receiver',
  templateUrl: './chat-message-Receiver.component.html',
  styleUrls: ['./chat-message-Receiver.component.css']
})
export class ChatMessageReceiverComponent implements OnInit, AfterViewInit {
  @Input()
  public message: ChatMessage;
  container: HTMLElement;

  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.container = document.getElementById("messages-container");
    this.container.scrollTop = this.container.scrollHeight;
  }

}
