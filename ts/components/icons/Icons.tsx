import * as React from 'react';
import Icon from './Icon';

export default class Icons {
    static get InboxIcon(): JSX.Element {
        let strokeWidth= 2;
        return (
            <Icon className="inbox-icon sidebar-icon" width={25} height={25}>
                <path
                    d={`M 5 40
                        l 45 25
                        l -45 25`}
                    fill="rgba(21,221,221, 0.75)"
                />
                <path
                    d={`M 95 40
                        l -45 25
                        l 45 25`
                    }
                    fill="rgba(21,221,221, 0.75)"
                />
                <path
                    d={`M 5 40
                        l 45 -30
                        l 45 30`}
                    fill="#fff"
                />
                <path
                    d={`M 5 90
                        L 95 90`}
                    stroke="rgba(21,221,221, 0.75)"
                    strokeWidth={2}
                />
            </Icon>
        );
    }

    static get SentIcon(): JSX.Element {
        let fill = '#fff'
        return (
            <Icon className="sent-icon" height={25} width={25}>
                <rect
                    x="0"
                    y="75"
                    width="75"
                    height="25"
                    fill={fill}
                />
                <path
                    d={`M 75 75
                        l 0 -15
                        l -8 0`}
                    fill="transparent"
                    stroke={fill}
                    strokeWidth={2}
                    />
                <path
                    d={`M 50 90
                        l 0 -50
                        l -20 0
                        l 0 -20
                        l 20 0
                        l 0 75`}
                    fill="#e82d43"
                    stroke="#e82d43"
                    strokeWidth="10"
                    />
            </Icon>
        )
    }

    static letters(text: string, className: string): JSX.Element {
        return (
            <Icon className={`letter-icon ${className}`} height={25} width={25}>
                <text x="50" y="75" textAnchor="middle">
                {text.toUpperCase()}
                </text>
            </Icon>
        );
    }

    static get TripsIcon(): JSX.Element {
        return (
            <Icon className="trips-icon sidebar-icon" width={25} height={25}>
                <rect
                    width="90"
                    height="60"
                    x="5"
                    y="35"
                    rx="5"
                    ry="5"
                    fill="tan"
                    stroke="brown"
                    strokeWidth="5"
                />
                <path
                    d={`M 40 35
                    l 0 -10
                    c 0 0 2 -2 2 -2
                    l 20 0
                    c 0 0 2 2 2 2
                    l 0 10`}
                    fill="transparent"
                    stroke="brown"
                    strokeWidth="5"
                />
                <path
                    d={`M 40 35
                    l 0 60
                    l -20 0
                    l 0 -60`}
                    fill="brown"
                />
                <path
                    d={`M 60 35
                    l 0 60
                    l 20 0
                    l 0 -60`}
                    fill="brown"
                />
                <path
                    d={`M 68 37
                    l 0 55
                    l 5 0
                    l 0 -55`}
                    fill="#fff"
                />
                <path
                    d={`M 27 37
                    l 0 55
                    l 5 0
                    l 0 -55`}
                    fill="#fff"
                />
            </Icon>
        );
    }

    static get SearchIcon(): JSX.Element {
        return (
            <Icon className="search-icon" width={20} height={20}>
                <circle
                    cx="65"
                    cy="35"
                    r="25"
                    strokeWidth="10"
                    stroke="#fff"
                    fill="transparent"
                />
                <path
                    d={`M 50 50
                    l -35 35`}
                    fill="transparent"
                    stroke="#fff"
                    strokeWidth="10"
                />
            </Icon>
        );
    }

}
