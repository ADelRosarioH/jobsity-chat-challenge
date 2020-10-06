import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { ChatMessage } from '../services/chat.service';

@Component({
  selector: 'app-chat-message-receiver',
  templateUrl: './chat-message-receiver.component.html',
  styleUrls: ['./chat-message-receiver.component.css']
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
