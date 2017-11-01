﻿#pragma checksum "..\..\NewEventWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8C76A8EAA84821591F7639E426FC118F27F40DE4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using citadel_wpf;


namespace citadel_wpf {
    
    
    /// <summary>
    /// NewEventWindow
    /// </summary>
    public partial class NewEventWindow : citadel_wpf.NewEntityWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 36 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock event_name;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox name_text;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock event_location;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox location_combo_box;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock event_unit_date_text;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox event_unit_date_number;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock event_date_text;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox event_date_number;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox notes_text;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\NewEventWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock required_text;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/citadel_wpf;component/neweventwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\NewEventWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.event_name = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.name_text = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.event_location = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.location_combo_box = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            
            #line 42 "..\..\NewEventWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Add_New_Location);
            
            #line default
            #line hidden
            return;
            case 6:
            this.event_unit_date_text = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.event_unit_date_number = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.event_date_text = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.event_date_number = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.notes_text = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.required_text = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            
            #line 57 "..\..\NewEventWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Cancel_and_Close);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 58 "..\..\NewEventWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Save);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

