import styled from "styled-components";
import Message from "./Message"


const Style = styled.div`
border: 2px solid #3396ff;
border-radius: 25px;
border-style: solid;
top: 20%;
left: 20%;
position: relative;
width: 300px;
height: 70%;
max-height: 1000px;
padding: 50px;
background: #f2ffe5;
`
const Log = () => {
    return(
<Style>
    <Message/>
    <Message/>
    <Message/>
    <Message/>
    <Message/>
    <Message/>
    <Message/>
    <Message/>
    <Message/>
    <Message/>
    <Message/>
</Style>
    )
}



export default Log