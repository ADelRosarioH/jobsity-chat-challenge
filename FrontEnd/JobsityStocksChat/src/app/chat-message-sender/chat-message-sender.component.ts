import { Component, Input, OnInit } from '@angular/core';
import { ChatMessage } from '../services/chat.service';

@Component({
  selector: 'app-chat-message-sender',
  templateUrl: './chat-message-sender.component.html',
  styleUrls: ['./chat-message-sender.component.css']
})
export class ChatMessageSenderComponent implements OnInit {

  @Input()
  message: ChatMessage;

  constructor() { }

  ngOnInit(): void {
  }

}
