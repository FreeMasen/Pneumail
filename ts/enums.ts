export enum SideBarState {
    Open = 'open',
    Closed = 'closed'
}

export enum IconPosition {
    Right = 'right',
    Left = 'left'
}

export enum DBWorkerState {
    Ready = 'ready',
    NotReady = 'not-ready',
    Error = 'error',
    NewMessage = 'new-message',
}

export enum UpdateType {
    None = 0,
    Initial = 1,
    Insert = 2,
    Delete = 4,
    Modify = 8,
    ServiceUpdateConfirmation = 16,
    RuleUpdateConfirmation = 32
}

export enum SearchLocation {
    Subject = 1,
    Body = 2,
    From = 4,
    To = 8,
}