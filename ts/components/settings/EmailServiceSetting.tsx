import * as React from 'react';
import TextBox from '../textBox/TextBox';

interface IEmailServiceSettingState {
    isError: boolean;
    inboundAddress: string;
    inboundPort: string;
    outboundAddress: string;
    outboundPort: string;
    username: string;
    password: string;
    confirm: string;
}

interface IEmailServiceSettingProps {
    service: IEmailService;
    error?: (msg: string) => void;
    update?: (newService: IEmailService) => void;
}

export default class EmailServiceSetting extends React.Component<IEmailServiceSettingProps, IEmailServiceSettingState> {


    constructor(props) {
        super(props);
        this.state = {
            isError: false,
            inboundAddress: props.service.inboundAddress,
            outboundAddress: props.service.outboundAddress,
            inboundPort: props.service.inboundPort,
            outboundPort: props.service.outboundPort,
            username: props.service.username || '',
            password: props.service.password || '',
            confirm: '',
        }
    }

    render() {
        return (
        <div className={`${this.state.isError ? 'error ' : ''}email-service-container paper`}>
            <div className="email-service-input-container">
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-address")`} className="input-label">Inbound Address</label>
                    <TextBox
                        initialValue={this.props.service.inboundAddress}
                        onChange={value => this.saveUpdate('inboundAddress', value) }
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-port")`} className="input-label">Inbound Port</label>
                    <TextBox
                        initialValue={this.props.service.inboundPort.toString()}
                        onChange={value => this.saveUpdate('inboundPort', value)}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-address")`} className="input-label">Outbound Address</label>
                    <TextBox
                        initialValue={this.props.service.outboundAddress}
                        onChange={value => this.saveUpdate('outboundAddress', value) }
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-port")`} className="input-label">Outbound Port</label>
                    <TextBox
                        initialValue={this.props.service.outboundPort.toString()}
                        onChange={value => this.saveUpdate('outboundPort', value)}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-username")`} className="input-label">Username</label>
                    <TextBox
                        initialValue={this.props.service.username}
                        onChange={value => this.saveUpdate('username', value)}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                    />
                </div>
                <div className="input-group">
                    <label htmlFor={`${this.props.service.id}-password")`} className="input-label">Password</label>
                    <TextBox
                        initialValue=""
                        onChange={value => this.saveUpdate('password', value)}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                        isPassword={true}
                    />
                </div>
                <div>
                    <label htmlFor={`${this.props.service.id}-confirm-password`} className="input-label">Confirm Password</label>
                    <TextBox
                        initialValue=""
                        onChange={value => this.saveUpdate('confirm', value)}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"
                        isPassword={true}
                    />
                </div>
            </div>
            <button onClick={ev => this.saveService(ev)} className="form-button submit">Save</button>
        </div>
        );
    }

    saveUpdate(key: string, value: string | number) {
        this.setState((prev, props) => {
            let ret = {} as any;
            ret[key] = value;
            return ret;
        });
    }

    saveService = (ev: React.MouseEvent<HTMLButtonElement>) => {
        let service = {} as IEmailService;
        service.id = this.props.service.id;
        service.inboundAddress = this.state.inboundAddress;
        service.outboundAddress = this.state.outboundAddress;
        try {
            service.inboundPort = parseInt(this.state.inboundPort);
            service.outboundPort = parseInt(this.state.outboundPort);
        } catch (e) {
            this.updateError(true);
            return this.props.error(e.message);
        }
        service.username = this.state.username;
        if (this.state.password !== this.state.confirm) {
            this.updateError(true);
            return this.props.error('Passwords must match');
        }
        service.password = this.state.password;
        this.props.update(service);
        this.updateError(false);
    }

    updateError = (isError: boolean) => {
        return this.setState((prev, props) => {
            return {
                isError
            }
        });
    }
}
